export interface PurchaseOrder {
  number: string;
  amount: number;
  receivedDate: Date;
  dueDate: Date;
  paidDate: Date;
  productName: string;
  customerName: string;
}
