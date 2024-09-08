import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { Observable } from 'rxjs';
import { HtmlResponse } from 'src/app/core/models/HtmlResponse';
import { AuthDetails } from 'src/app/shared/models/authDetails';
import { Root } from 'src/app/shared/models/baseResponse';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { Viourtn, iouRtn } from '../iouRtn';

@Injectable({
  providedIn: 'root'
})
export class IoureturnService {
	baseUrl = environment.apiUrl;
	constructor(private http: HttpClient) {}

	getiouRtn(): Observable<Root<Viourtn[]>> {
		const auth: AuthDetails = getAuthDetails();
		return this.http.post<Root<Viourtn[]>>(this.baseUrl + "iouRtn/getiouRtn", auth);
	}

	addiouRtn(iouRtn: iouRtn) {
		return this.http.post<Root<iouRtn[]>>(
			this.baseUrl + 'iouRtn/insertiouRtn',
			iouRtn
		);
	}

	updateiouRtn(iouRtn: iouRtn) {
		return this.http.post<Root<iouRtn[]>>(
			this.baseUrl + 'iouRtn/updateiouRtn',
			iouRtn
		);
	}

	generateReports(): Observable<HtmlResponse> {
		return this.http.get<HtmlResponse>(this.baseUrl + 'iouRtn/report');
	}
	  
	cellreport(id: number): Observable<any> {
		return this.http.get(this.baseUrl + 'iouRtn/cellreport/' + id);
	}
}
