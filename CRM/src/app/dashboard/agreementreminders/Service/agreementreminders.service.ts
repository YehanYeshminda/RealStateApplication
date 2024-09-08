import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { Observable } from 'rxjs';
import { HtmlResponse } from 'src/app/core/models/HtmlResponse';
import { AuthDetails } from 'src/app/shared/models/authDetails';
import { Root } from 'src/app/shared/models/baseResponse';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { ComboInfo } from 'src/app/shared/models/comboInfo';
import { Vagree, agree } from '../agreementremider';

@Injectable({
  providedIn: 'root'
})
export class AgreementremindersService {
	baseUrl = environment.apiUrl;
	constructor(private http: HttpClient) {}

	getagreerem(): Observable<Root<Vagree[]>> {
		const auth: AuthDetails = getAuthDetails();
		return this.http.post<Root<Vagree[]>>(this.baseUrl + "AgreementReminder/getagreementreminder", auth);
	}

	getStaffNoAndIdComboData(): Observable<ComboInfo[]> {
		const auth: AuthDetails = getAuthDetails();
		return this.http.post<ComboInfo[]>(this.baseUrl + "AgreementReminder/GetStaffNameIdAll", auth)
	}

	addagreerem(agreerem: agree) {
		return this.http.post<Root<agree[]>>(
			this.baseUrl + 'AgreementReminder/insertagreementreminder',
			agreerem
		);
	}

	updateagreerem(agreerem: agree) {
		return this.http.post<Root<agree[]>>(
			this.baseUrl + 'AgreementReminder/updateagreementreminder',
			agreerem
		);
	}

	generateReports(): Observable<HtmlResponse> {
		return this.http.get<HtmlResponse>(this.baseUrl + 'AgreementReminder/report');
	}
	  
	cellreport(id: number): Observable<any> {
		return this.http.get(this.baseUrl + 'AgreementReminder/cellreport/' + id);
	}
}
