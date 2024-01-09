export class APIs {
  //Auth
  static signinApi = '/Auth/signin';
  static signupApi = '/Auth/register';
  static refreshTokenApi = '/Auth/refreshToken';
  static forgotPasswordApi = '/Auth/forgotPassword';
  static resetPasswordApi = '/Auth/resetPassword';
  static changePasswordApi = '/Auth/changePassword';

  //User
  static searchUserApi = '/Users/search';
  static inviteUserApi = '/Users/invite-user';
  static acceptInviteUserApi = '/Users/accept-invite';
  static deleteUserApi = '/Users/';
  static getUserApi = '/Users/';
  static updateUserApi = '/users/';
  static getInviteDetails = '/users/get-invite-details';
  static getProfileDetails = '/users/get-profile-details';
  static updateProfile = '/users/update-profile/';

  //Projects
  static searchProjectsApi = '/project/search';
  static deleteProjectApi = '/project/';
  static getProjectApi = '/project/';
  static updateProjectApi = '/project/';
  static createProjectApi = '/project/Create';

  //Tasks
  static searchTasksApi = '/Task/search';
  static deleteTaskApi = '/Task/';
  static getTaskApi = '/Task/';
  static updateTaskApi = '/Task/';
  static createTaskApi = '/Task/Create';
  static getTaskStatusListApi = '/Task/status-list';
  static getTaskPriorityListApi = '/Task/priority-list';
  static getAssigneeListApi = '/Task/assignee-list';
  static getProjectListApi = '/Task/projects';
}
