import { Permissions, PermissionValue } from '../modules/permissions/permissions.model';

export type PartialPermissions = {
  [key in keyof Partial<Permissions>]: Partial<PermissionValue>;
};

export function accessAvailable(expectedPermissions: PartialPermissions, actualPermissions: PartialPermissions): boolean {
  return (Object.entries(expectedPermissions) as [keyof Permissions, Partial<PermissionValue>][]).every(([key, value]) =>
    (Object.entries(value) as [keyof PermissionValue, boolean][]).every(([_key, _value]) => {
      const actual = actualPermissions[key];

      return actual && actual[_key] === _value;
    }),
  );
}

export function permissionsFrom(from: PartialPermissions): PartialPermissions {
  return from;
}
