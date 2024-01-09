export class Regex {
  static passwordValidationPattern =
    '^(?=.*[A-Z])(?=.*[a-z])(?=.*[^a-zA-Z0-9]).{8,}$';
  static noSpaceValidationPattern = '^\\S*$';
  static decimalValidationPattern = '^\\d+(\\.\\d{1,2})?$';
}

export class StorageKey {
  static tokenKey = 'authToken';
  static refreshTokenKey = 'refreshToken';
  static userInfo = 'userInfo';
}

export class AlertNotification {
  static type = {
    success: 'success',
    warning: 'warning',
    info: 'info',
    error: 'error',
  };
}

export class FieldValidation {
  static firstNameMaxLength = 20;
  static lastNameMaxLength = this.firstNameMaxLength;
  static emailMaxLength = 50;
  static passwordMinLength = 6;
}

export const queryStringParams = {
  UserId: 'userId',
  Email: 'email',
  Token: 'token',
};
