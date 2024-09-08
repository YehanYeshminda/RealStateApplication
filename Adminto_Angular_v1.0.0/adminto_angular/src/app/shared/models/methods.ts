export function GetAuthDetails(): AuthDetails {
    const currentUserString: string | null = sessionStorage.getItem('currentUser');
    if (currentUserString) {
        const data: UserSessionData = JSON.parse(currentUserString);
        return data.result;
    } else {
        throw new Error('currentUser is not available.');
    }
}

export function formatDateReset(inputDate: Date): string {
    const year = inputDate.getFullYear();
    const month = (inputDate.getMonth() + 1).toString().padStart(2, '0');
    const day = inputDate.getDate().toString().padStart(2, '0');
    return `${year}-${month}-${day}`;
}



interface UserSessionData {
    isSuccess: boolean
    message: string
    refNo: string
    result: AuthDetails
}

export interface AuthDetails {
    hash: string
}