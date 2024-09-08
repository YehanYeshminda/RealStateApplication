export interface DynamicTable {
  id: number;
  dynamicField: string;
  remark: string;
  status: number;
  cId: number;
}

export interface DynamicExpenseTable {
  id: number;
  mainCategory: string;
  subCategory: string;
  status: number;
}
