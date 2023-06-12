import { User } from '../user/user.model';
import { Permissions } from '../permissions/permissions.model';

export interface LoginResponse {
  access_token: string;
  exp: number;
  user: User;
  permissions: Permissions;
}
