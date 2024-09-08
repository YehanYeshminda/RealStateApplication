import { Observable, map } from "rxjs";
import { CommonHttpService } from "../services/common-http.service";
import { CheckForPermission } from "./models/models";
import { GetAuthDetails } from "src/app/shared/models/methods";

export function checkForAccess(commonHttpService: CommonHttpService, event: string, location: string): Observable<boolean> {
    const data: CheckForPermission = {
        authDto: GetAuthDetails(),
        event: event,
        location: location
    };

    return commonHttpService.checkForAccess(data).pipe(
        map(response => response.hasAccess === '1')
    );
}

export function formatDate(inputDate: Date): string {
    const year = inputDate.getFullYear();
    const month = (inputDate.getMonth() + 1).toString().padStart(2, '0');
    const day = inputDate.getDate().toString().padStart(2, '0');
    return `${year}-${month}-${day}`;
}

export function extractDate(timestamp: string): string {
    const dateRegex = /(\d{1,2}\/\d{1,2}\/\d{4})/;
    const match = timestamp.match(dateRegex);
  
    if (match && match.length > 0) {
      return match[0];
    } else {
      return '';
    }
  }