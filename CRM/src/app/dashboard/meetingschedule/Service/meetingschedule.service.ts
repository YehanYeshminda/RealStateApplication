import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { Observable } from 'rxjs';
import { HtmlResponse } from 'src/app/core/models/HtmlResponse';
import { AuthDetails } from 'src/app/shared/models/authDetails';
import { Root } from 'src/app/shared/models/baseResponse';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { meetsched, vmeeting } from '../meeting';
import { ComboInfo, ComboInfoBank } from 'src/app/shared/models/comboInfo';

@Injectable({
  providedIn: 'root'
})
export class MeetingscheduleService {
	baseUrl = environment.apiUrl;
	constructor(private http: HttpClient) {}

	getmeetsched(): Observable<Root<vmeeting[]>> {
		const auth: AuthDetails = getAuthDetails();
		return this.http.post<Root<vmeeting[]>>(this.baseUrl + "meetingscheduler/getMeeting", auth);
	}

	getStaffNoAndIdComboData(): Observable<ComboInfo[]> {
		const auth: AuthDetails = getAuthDetails();
		return this.http.post<ComboInfo[]>(this.baseUrl + "meetingscheduler/GetStaffNameIdAll", auth)
	}

	addmeetsched(meetsched: meetsched) {
		return this.http.post<Root<meetsched[]>>(
			this.baseUrl + 'meetingscheduler/insertMeeting',
			meetsched
		);
	}

	updatemeetsched(meetsched: meetsched) {
		return this.http.post<Root<meetsched[]>>(
			this.baseUrl + 'meetingscheduler/updateMeeting',
			meetsched
		);
	}

	generateReports(): Observable<HtmlResponse> {
		return this.http.get<HtmlResponse>(this.baseUrl + 'meetingscheduler/report');
	}
	  
	cellreport(id: number): Observable<any> {
		return this.http.get(this.baseUrl + 'meetingscheduler/cellreport/' + id);
	}
}
