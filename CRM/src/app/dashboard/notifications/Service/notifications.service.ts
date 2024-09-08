import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { Observable } from 'rxjs';
import { HtmlResponse } from 'src/app/core/models/HtmlResponse';
import { AuthDetails } from 'src/app/shared/models/authDetails';
import { Root } from 'src/app/shared/models/baseResponse';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { ComboInfo } from 'src/app/shared/models/comboInfo';
import { notification, vNofitication } from '../notifications';

@Injectable({
	providedIn: 'root'
})
export class NotificationsService {
	baseUrl = environment.apiUrl;
	constructor(private http: HttpClient) { }

	getnotification(): Observable<Root<vNofitication[]>> {
		const auth: AuthDetails = getAuthDetails();
		return this.http.post<Root<vNofitication[]>>(this.baseUrl + "Notification/getnotification", auth);
	}

	getStaffNoAndIdComboData(): Observable<ComboInfo[]> {
		const auth: AuthDetails = getAuthDetails();
		return this.http.post<ComboInfo[]>(this.baseUrl + "Notification/GetStaffNameIdAll", auth)
	}

	addnotification(notification: notification) {
		return this.http.post<Root<notification[]>>(
			this.baseUrl + 'Notification/insertnotification',
			notification
		);
	}

	updatenotification(notification: notification) {
		return this.http.post<Root<notification[]>>(
			this.baseUrl + 'Notification/updatenotification',
			notification
		);
	}

	generateReports(): Observable<HtmlResponse> {
		return this.http.get<HtmlResponse>(this.baseUrl + 'Notification/report');
	}

	cellreport(id: number): Observable<any> {
		return this.http.get(this.baseUrl + 'Notification/cellreport/' + id);
	}
}
