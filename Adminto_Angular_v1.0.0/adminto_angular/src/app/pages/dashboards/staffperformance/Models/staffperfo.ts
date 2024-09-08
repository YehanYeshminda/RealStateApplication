export interface StaffPerformancePaginationResult {
    data: StaffPerformance[]
    totalCountPages: number
    totalData: number
}
  
export interface StaffPerformance {
    staffName: string
    leadConvertedCount: number
    callMadeCount: number
    meetingsPlanned: number
}

export interface StaffPerformanceRequest {
    hash: string
    pageSize: number
    page: number
}


  export interface PaginationResult {
    staffName: string
    leadConvertedCount: number
    callMadeCount: number
    meetingsPlanned: number
  }