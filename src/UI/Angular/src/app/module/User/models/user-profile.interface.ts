export interface IUserProfile {
  id: string;
  firstName?: string;
  lastName?: string;
  email: string;
  imageUrl?: string;
}

export interface IUserProfileSignal {
  name: string;
  email: string;
  imageUrl?: string;
}
