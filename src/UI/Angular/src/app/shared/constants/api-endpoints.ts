export class APIs {
  //Auth
  static signinApi = '/Auth/signin';
  static signupApi = '/Auth/register';
  static refreshTokenApi = '/Auth/refreshToken';
  static forgotPasswordApi = '/Auth/forgotPassword';
  static reserpasswordApi = '/Auth/resetPassword';
  static changepasswordApi = '/Auth/changePassword';

  //User
  static searchUserApi = '/Users/search';
  static inviteUserApi = '/Users/invite-user';
  static acceptInviteUserApi = '/Users/accept-invite';
  static deleteUserApi = '/Users/';
  static getUserApi = '/Users/';
  static updateUserApi = '/users/';
  static getInviteDetails = '/users/get-invite-details';
}
