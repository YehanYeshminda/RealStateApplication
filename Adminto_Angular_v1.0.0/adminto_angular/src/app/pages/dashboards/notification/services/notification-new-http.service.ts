import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Root } from 'src/app/shared/models/base';
import { AuthDetails, GetAuthDetails } from 'src/app/shared/models/methods';
import { environment } from 'src/environments/environment';
import { MessageResponse, MessageResponsePaginationResult, notificationdto } from '../notification-list/models/message';
import { MakeCenterPaginationRequest } from '../../call-center/make-calls/models/ makecall';
import { ComboInfo } from '../../shared/models/models';

@Injectable({
  providedIn: 'root'
})
export class NotificationNewHttpService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getAllNotifications(data: MakeCenterPaginationRequest): Observable<Root<MessageResponsePaginationResult>> {
    return this.http.post<Root<MessageResponsePaginationResult>>(this.baseUrl + "Notification/GetAllNotifications", data);
  }

  // addNewNotification(data: notificationdto): Observable<Root<notificationdto>> {
  //   return this.http.post<Root<notificationdto>>(this.baseUrl + "Notification/InsertProcedureNotification", data)
  // }

  getStaffNoAndIdComboData(): Observable<ComboInfo[]> {
		const auth: AuthDetails = GetAuthDetails();
		return this.http.post<ComboInfo[]>(this.baseUrl + "Notification/GetStaffNameIdAll", auth)
	}

	addnotification(notification: notificationdto) {
		return this.http.post<Root<notificationdto[]>>(
			this.baseUrl + 'Notification/InsertProcedureNotification',
			notification
		);
	}

	updatenotification(notification: MessageResponse) {
		return this.http.post<Root<MessageResponse[]>>(
			this.baseUrl + 'Notification/UpdateProcedureNotification',
			notification
		);
	}
  // editExistingNotification(data: notificationdto): Observable<Root<notificationdto>> {
  //   return this.http.post<Root<notificationdto>>(this.baseUrl + "Notification/update", data)
  // }


	deletenotification(id: number): Observable<Root<String>> {
		const auth: AuthDetails = GetAuthDetails();
		return this.http.post<Root<string>>(this.baseUrl + "Notification/DeleteNotification?id=" + id, auth);
	}
}
