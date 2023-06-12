import { Permissions } from '../permissions/permissions.model';

export interface AddUserRequest {
  username: string;
  password: string;
  permissions: Permissions;
}
