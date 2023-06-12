export interface AddInvoiceRequest {
  productName: string;
  customerName: string;
  purchaseOrderNumber: string;
  number: string;
  amount: number;
  receivedDate: string;
  dueDate: string;
}
