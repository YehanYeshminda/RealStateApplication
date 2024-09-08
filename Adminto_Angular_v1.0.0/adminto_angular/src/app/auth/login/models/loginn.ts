export interface LoginUserRequest {
    branchId: number
    username: string
    password: string
}

export interface LoginUserResponse {
    hash: string
}