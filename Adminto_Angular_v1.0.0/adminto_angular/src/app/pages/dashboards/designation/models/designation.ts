export interface DesignationViewData {
    data: DesignationAll[]
    totalCountPages: number
    totalData: number
  }
  
  export interface DesignationAll {
    typeId: number
    typeName: string
    remark: string
    status: number
    cid?: number
  }