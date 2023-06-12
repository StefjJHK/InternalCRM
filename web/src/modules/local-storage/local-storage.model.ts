import { User } from '../user/user.model';
import { Permissions } from '../permissions/permissions.model';

export interface LocalStorage {
  user?: User;
  permissions?: Permissions;
}
