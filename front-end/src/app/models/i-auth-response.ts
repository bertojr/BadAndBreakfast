import { iUser } from './i-user';

export interface iAuthResponse {
  token: string;
  user: iUser;
}
