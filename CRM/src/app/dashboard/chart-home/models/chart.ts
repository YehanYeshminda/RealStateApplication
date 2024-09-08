export interface ChartData {
    labels: string[]
    datasets: number[]
}

export interface CalenderData {
    id: string
    title: string
    startDate: string
}

export interface DashboaordLeadCount {
    openLeadCount: number
    allLeadCount: number
    hotLeadCount: number
    conversions: number
}