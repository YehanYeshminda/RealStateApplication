
export interface AssignedCallsPersonInsigntResultAll {
    data: AssignedCallsPersonInsignt[]
    totalCountPages: number
    totalData: number
}
  
export interface AssignedCallsPersonInsignt {
    id: number
    firstName: string
    lastName: string
    email: string
    phoneNo: string
    phoneNo2: string
    assignedTo: string
    addOn: string
    calledOn: any
    callEndedOn: any
    status: number
    assignedOn: string
}