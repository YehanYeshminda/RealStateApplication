import { CommonHttpService } from "src/app/dashboard/common/common-http.service";
import { AuthDetails } from "../models/authDetails";
import { Observable, map } from "rxjs";
import { CheckForPermission } from "src/app/dashboard/common/models/common";

export function getAuthDetails() {
    const userInfo = JSON.parse(sessionStorage.getItem('user') || 'null');
    const authCredentials = userInfo.hash;
    const auth: AuthDetails = {
        hash: authCredentials,
    };

    return auth;
}

export function formatDate(inputDate: Date): string {
    const year = inputDate.getFullYear();
    const month = (inputDate.getMonth() + 1).toString().padStart(2, '0');
    const day = inputDate.getDate().toString().padStart(2, '0');
    return `${year}-${month}-${day}`;
}

export function formatDateReset(inputDate: Date): string {
    const year = inputDate.getFullYear();
    const month = (inputDate.getMonth() + 1).toString().padStart(2, '0');
    const day = inputDate.getDate().toString().padStart(2, '0');
    return `${year}-${month}-${day}`;
}

export function checkForAccess(commonHttpService: CommonHttpService, event: string, location: string): Observable<boolean> {
    const data: CheckForPermission = {
        authDto: getAuthDetails(),
        event: event,
        location: location
    };

    return commonHttpService.checkForAccess(data).pipe(
        map(response => response.hasAccess === '1')
    );
}

export const DECIMAL_REGEX = /^[0-9]+(\.[0-9]+)?$/;