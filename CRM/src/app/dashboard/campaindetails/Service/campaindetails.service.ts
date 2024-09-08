import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { Observable } from 'rxjs';
import { HtmlResponse } from 'src/app/core/models/HtmlResponse';
import { AuthDetails } from 'src/app/shared/models/authDetails';
import { Root } from 'src/app/shared/models/baseResponse';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { ComboInfoBank } from 'src/app/shared/models/comboInfo';
import { campaindetails } from '../campaindetails';

@Injectable({
  providedIn: 'root'
})
export class CampaindetailsService {
	baseUrl = environment.apiUrl;
	constructor(private http: HttpClient) {}

	getcampaindetails(): Observable<Root<campaindetails[]>> {
		const auth: AuthDetails = getAuthDetails();
		return this.http.post<Root<campaindetails[]>>(this.baseUrl + "CampainH/getCampainH", auth);
	}

	getMediaNoAndIdComboData(): Observable<ComboInfoBank[]> {
		const auth: AuthDetails = getAuthDetails();
		return this.http.post<ComboInfoBank[]>(this.baseUrl + "CampainH/GetMediaIdAll", auth)
	}

	addcampaindetails(campaindetails: campaindetails) {
		return this.http.post<Root<campaindetails[]>>(
			this.baseUrl + 'CampainH/insertCampainH',
			campaindetails
		);
	}

	updatecampaindetails(campaindetails: campaindetails) {
		return this.http.post<Root<campaindetails[]>>(
			this.baseUrl + 'CampainH/updateCampainH',
			campaindetails
		);
	}

	generateReports(): Observable<HtmlResponse> {
		return this.http.get<HtmlResponse>(this.baseUrl + 'CampainH/report');
	}
	  
	cellreport(id: string): Observable<any> {
		return this.http.get(this.baseUrl + 'CampainH/cellreport/' + id);
	}
}
