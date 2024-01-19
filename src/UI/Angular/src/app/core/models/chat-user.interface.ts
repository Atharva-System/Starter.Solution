export interface IChatUser {
  userId: string;
  name: string;
  path: string;
  time: string;
  date: string;
  preview: string;
  messages: IChatMessage[];
  active: boolean;
  connectionId: string;
}

export interface IChatMessage {
  fromUserId: string;
  toUserId: string;
  text: string;
  time: string;
  date: string;
}

export interface IUserTyping {
  typingBy: string;
  isTyping: boolean;
}
