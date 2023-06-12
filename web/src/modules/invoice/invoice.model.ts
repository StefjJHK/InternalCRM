export interface Invoice {
  number: string;
  amount: number;
  receivedDate: Date;
  dueDate: Date;
  paidDate: Date;
  purchaseOrderNumber: string;
  customerName: string;
  productName: string;
}
