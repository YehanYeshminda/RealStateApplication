import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Root } from 'src/app/shared/models/base';
import { AuthDetails, GetAuthDetails } from 'src/app/shared/models/methods';
import { environment } from 'src/environments/environment';
import { HtmlResponse } from '../../staff/models/staff';
import { VVTSListAll, VtsView, vts } from '../Modals/vts';

@Injectable({
	providedIn: 'root'
})
export class VendertoserviceService {

	baseUrl = environment.apiUrl;
	constructor(private http: HttpClient) { }

	// getVTS(auth: AuthDetails): Observable<Root<VtsView[]>> {
	// 	return this.http.post<Root<VtsView[]>>(
	// 		this.baseUrl + 'vendertoservice/GetAllData',
	// 		auth
	// 	);
	// }

	getVTS(page: number = 1, pageSize: number = 2): Observable<Root<VVTSListAll>> {
		const auth: AuthDetails = GetAuthDetails();
		return this.http.post<Root<VVTSListAll>>(this.baseUrl + "vendertoservice/GetAllData", auth, {
			params: {
				page: page,
				pageSize: pageSize
			}
		});
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
