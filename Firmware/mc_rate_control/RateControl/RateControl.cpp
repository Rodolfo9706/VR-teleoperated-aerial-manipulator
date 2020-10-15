/****************************************************************************
 *
 *   Copyright (c) 2019 PX4 Development Team. All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 *
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in
 *    the documentation and/or other materials provided with the
 *    distribution.
 * 3. Neither the name PX4 nor the names of its contributors may be
 *    used to endorse or promote products derived from this software
 *    without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
 * "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
 * LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS
 * FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE
 * COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING,
 * BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS
 * OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED
 * AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
 * LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN
 * ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
 * POSSIBILITY OF SUCH DAMAGE.
 *
 ****************************************************************************/

/**
 * @file RateControl.cpp
 */

#include <RateControl.hpp>
#include <px4_platform_common/defines.h>



using namespace matrix;

void RateControl::setGains(const Vector3f &P, const Vector3f &I, const Vector3f &D)
{
	_gain_p = P;
	_gain_i = I;
	_gain_d = D;
}

void RateControl::setSaturationStatus(const MultirotorMixer::saturation_status &status)
{
	_mixer_saturation_positive[0] = status.flags.roll_pos;
	_mixer_saturation_positive[1] = status.flags.pitch_pos;
	_mixer_saturation_positive[2] = status.flags.yaw_pos;
	_mixer_saturation_negative[0] = status.flags.roll_neg;
	_mixer_saturation_negative[1] = status.flags.pitch_neg;
	_mixer_saturation_negative[2] = status.flags.yaw_neg;
}

Vector3f RateControl::update(const vehicle_attitude_s &att, const vehicle_attitude_setpoint_s &att_sp, const Vector3f &rate, const Vector3f &rate_sp, const Vector3f &angular_accel,
			     const float dt, const bool landed)
{

//____________________________|MODIFICADO|____________________________________________________
    float roll_body = _vehicle_attitude_sp.roll_body;
    float pitch_body = _vehicle_attitude_sp.pitch_body;
    float yaw_body = _vehicle_attitude_sp.yaw_body;

    /* get attitude setpoint rotational matrix */
    Dcmf _rot_des = Eulerf(roll_body, pitch_body, yaw_body);

    /* get current rotation matrix from control state quaternions */
    Quatf q_att(att.q[0], att.q[1], att.q[2], att.q[3]);
    Matrix3f _rot_att = q_att.to_dcm();

    Vector3f e_R_vec;

    /* Compute matrix: attitude error */
    Matrix3f e_R = (_rot_des.transpose() * _rot_att - _rot_att.transpose() * _rot_des) * 0.5;

    Matrix3f omega_gorro;
    Matrix3f J;

    float phi = rate(0);
    float theta = rate(1);
    float psi = rate(2);

    omega_gorro(0,0) = 0;  /**< Roll  */
    omega_gorro(0,1)=  -psi;  /**< Pitch */
    omega_gorro(0,2) =  theta;  /**< Yaw   */

   omega_gorro (1,0) = psi ;
   omega_gorro (1,1) = 0 ;
   omega_gorro (1,2)= -phi;

   omega_gorro(2,0)=  -theta;
   omega_gorro(2,1) =  phi;
   omega_gorro(2,2) =  0;

   J(0,0) = 0.011;
   J(1,0) = 0;
   J(2,0) = 0;
   J(0,1) = 0;
   J(1,1) = 0.015;
   J(2,1) = 0;
   J(0,2) = 0;
   J(1,2) = 0;
   J(2,2) = 0.021;


    //Vector3f e_omega = rate - (_rot_att.transpose()*_rot_des)*rate_sp;

   Vector3f e_omega = rate -rate_sp;



    float a,b,c,e,f,g;

     Vector3f operacion = J*rate ;

     a=rate(0);
     b=rate(1);
     c=rate(2);

     e=operacion(0);
     f=operacion(1);
     g=operacion(2);

     Vector3f cross_product;

     cross_product(0)=(b*g-f*c);
     cross_product(1)=(a*g-e*c);
     cross_product(2)=(a*f-b*e);



    /* vee-map the error to get a vector instead of matrix e_R */
    e_R_vec(0) = e_R(2, 1);  /**< Roll  */
    e_R_vec(1) = e_R(0, 2);  /**< Pitch */
    e_R_vec(2) = e_R(1, 0);  /**< Yaw   */

    Vector3f torque;

    Vector3f omega_sp_derivada = (rate_sp - omega_sp_anterior)/dt;


    //Vector3f rate_error = rate - rate_sp;



    const float p_roll = 1;
    const float d_roll = 0.1;

     const float p_pitch = 1.2 ;
     const float d_pitch = 0.1 ;


     const float p_yaw = 1.2;
     const float d_yaw = 0.1;

    /**< PD-Control */
    torque(0) = (- p_roll*e_R_vec(0)) - (d_roll*e_omega(0));	/**< Roll  */
    torque(1) = (- p_pitch*e_R_vec(1)) - (d_pitch*e_omega(1));	/**< Pitch */
    torque(2) = (- p_yaw*e_R_vec(2)) - (d_yaw*e_omega(2));   /**< Yaw   */


    torque = torque + cross_product-J*(omega_gorro*_rot_att.transpose()*_rot_des*rate_sp-_rot_att.transpose()*_rot_att*omega_sp_derivada);

//    const float p_roll = 1;
//    const float d_roll = 0.1;

//     const float p_pitch = 1.2 ;
//     const float d_pitch = 0.1 ;


//     const float p_yaw = 1.2;
//     const float d_yaw = 0.1 ;

    
    
//____________________________________________________________________________________________________

	// PID control with feed forward
        //const Vector3f torque = _gain_p.emult(rate_error) + _rate_int - _gain_d.emult(angular_accel) + _gain_ff.emult(rate_sp);

	// update integral only if we are not landed
	if (!landed) {
                updateIntegral(e_omega, dt);
	}



        omega_sp_anterior = rate_sp;
	return torque;
}

void RateControl::updateIntegral(Vector3f &rate_error, const float dt)
{
	for (int i = 0; i < 3; i++) {
		// prevent further positive control saturation
		if (_mixer_saturation_positive[i]) {
			rate_error(i) = math::min(rate_error(i), 0.f);
		}

		// prevent further negative control saturation
		if (_mixer_saturation_negative[i]) {
			rate_error(i) = math::max(rate_error(i), 0.f);
		}

		// I term factor: reduce the I gain with increasing rate error.
		// This counteracts a non-linear effect where the integral builds up quickly upon a large setpoint
		// change (noticeable in a bounce-back effect after a flip).
		// The formula leads to a gradual decrease w/o steps, while only affecting the cases where it should:
		// with the parameter set to 400 degrees, up to 100 deg rate error, i_factor is almost 1 (having no effect),
		// and up to 200 deg error leads to <25% reduction of I.
		float i_factor = rate_error(i) / math::radians(400.f);
		i_factor = math::max(0.0f, 1.f - i_factor * i_factor);

		// Perform the integration using a first order method
		float rate_i = _rate_int(i) + i_factor * _gain_i(i) * rate_error(i) * dt;

		// do not propagate the result if out of range or invalid
		if (PX4_ISFINITE(rate_i)) {
			_rate_int(i) = math::constrain(rate_i, -_lim_int(i), _lim_int(i));
		}
	}
}

void RateControl::getRateControlStatus(rate_ctrl_status_s &rate_ctrl_status)
{
	rate_ctrl_status.rollspeed_integ = _rate_int(0);
	rate_ctrl_status.pitchspeed_integ = _rate_int(1);
	rate_ctrl_status.yawspeed_integ = _rate_int(2);
}
