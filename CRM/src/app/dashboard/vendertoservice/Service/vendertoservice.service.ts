import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { GetDynamicFormRequest, SendDynamicFormRequest } from '../../dynamic-form/models/dynamicForm';
import { Observable } from 'rxjs';
import { AuthDetails } from 'src/app/shared/models/authDetails';
import { VtsView, vts } from '../vendertoservice';
import { HtmlResponse } from 'src/app/core/models/HtmlResponse';
import { UniquePK } from 'src/app/core/models/UniquePK';
import { Root } from 'src/app/shared/models/baseResponse';

@Injectable({
	providedIn: 'root'
})
export class VendertoserviceService {

	baseUrl = environment.apiUrl;
	constructor(private http: HttpClient) { }

	getVTS(auth: AuthDetails): Observable<Root<VtsView[]>> {
		return this.http.post<Root<VtsView[]>>(
			this.baseUrl + 'vendertoservice/GetAllData',
			auth
		);
	}

	addVTS(vender: vts) {
		return this.http.post<Root<vts[]>>(
			this.baseUrl + 'vendertoservice/Add',
			vender
		);
	}

	updateVTS(vender: vts) {
		return this.http.post<Root<vts[]>>(
			this.baseUrl + 'vendertoservice/update',
			vender
		);
	}

	generateReports(): Observable<HtmlResponse> {
		return this.http.get<HtmlResponse>(this.baseUrl + 'vendertoservice/report');
	}

	cellreport(id: number): Observable<any> {
		return this.http.get(this.baseUrl + 'vendertoservice/cellreport/' + id);
	}

}
