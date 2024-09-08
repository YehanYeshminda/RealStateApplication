export interface Message {
    id: number;
    avatar: string;
    sender: string;
    text: string;
    time: string;
}

export interface Project {
    id: number;
    name: string;
    startDate: string;
    dueDate: string;
    status: string;
    variant: string;
    assignee: string;
}

export interface ChartPieData {
    assignedCallsButCalled: number
    callsToMake: number
    conversions: number
}

export interface BarChartData {
    Sunday: number
    Monday: number
    Tuesday: number
    Wednesday: number
    Thursday: number
    Friday: number
    Saturday: number
}
