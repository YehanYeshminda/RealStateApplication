export interface vts {
    id: number;
    venderid: number;
    serviceid: number;
    status: number;
}

export interface VtsView {
    Id: number
    SupplierName: string
    TypeName: string
    Status: number
    AddOn: string
    AddBy: string
}



export interface VVTSListAll {
    data: VtsView[]
    totalCountPages: number
    totalData: number
  }