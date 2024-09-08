import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { Observable } from 'rxjs';
import { HtmlResponse } from 'src/app/core/models/HtmlResponse';
import { AuthDetails } from 'src/app/shared/models/authDetails';
import { Root } from 'src/app/shared/models/baseResponse';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { ComboInfo, ComboInfoBank } from 'src/app/shared/models/comboInfo';
import { meetsched, remeet, updatemeet, vmeeting } from '../../meetingschedule/meeting';

@Injectable({
  providedIn: 'root'
})
export class MeetingupdateService {
	baseUrl = environment.apiUrl;
	constructor(private http: HttpClient) {}

	getmeetupdate(): Observable<Root<vmeeting[]>> {
		const auth: AuthDetails = getAuthDetails();
		return this.http.post<Root<vmeeting[]>>(this.baseUrl + "meetingscheduler/getMeeting", auth);
	}

	getStaffNoAndIdComboData(): Observable<ComboInfo[]> {
		const auth: AuthDetails = getAuthDetails();
		return this.http.post<ComboInfo[]>(this.baseUrl + "meetingupdate/GetStaffNameIdAll", auth)
	}


	meetupdate(meetupdate: updatemeet) {
		return this.http.post<Root<updatemeet[]>>(
			this.baseUrl + 'meetingupdate/updateMeeting',
			meetupdate
		);
	}

  reupdate(meetupdate: remeet) {
		return this.http.post<Root<remeet[]>>(
			this.baseUrl + 'MeetingUpdate/rescheduleMeeting',
			meetupdate
		);
	}

	generateReports(): Observable<HtmlResponse> {
		return this.http.get<HtmlResponse>(this.baseUrl + 'meetingscheduler/report');
	}

  getmeetname(id: number): Observable<any> {
		return this.http.get(this.baseUrl + 'MeetingUpdate/Meetname' + id);
	}
}
