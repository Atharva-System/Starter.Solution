export interface TokenClaims {
  name: string;
  aud: string;
  email: string;
  exp: number;
  iss: string;
  jti: string;
  roles: string;
  sub: string;
  uid: string;
}
