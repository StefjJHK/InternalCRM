export interface PermissionValue {
  canWrite: boolean;
  canRead: boolean;
}

export interface Permissions {
  analytics: PermissionValue;
  product: PermissionValue;
  customer: PermissionValue;
  lead: PermissionValue;
  purchaseOrder: PermissionValue;
  invoice: PermissionValue;
  payment: PermissionValue;
  subscription: PermissionValue;
}
