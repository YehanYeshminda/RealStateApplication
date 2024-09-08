import { AuthDetails } from "src/app/shared/models/methods"

export interface ChartStatisticData {
    convertedLeadsCount: number
    conversionPercentage: number
}

export interface UpdateOldPassword {
  authDto: AuthDetails
  oldPassword: string
  password: string
}
  