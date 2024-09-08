import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment.development';
import { ComboInfo } from '../models/combo';
import { DynamicTable } from '../models/dynamic';

export interface RequestBodyCombo {
	query: string;
}

export interface GetCatergoryTypeById {
	id: number
	subCategory: string
	remark: string
	status: number
	cid?: number
	catergoryId: number
}

@Injectable({
	providedIn: 'root',
})
export class CommonHttpServiceService {
	baseUrl = environment.apiUrl;

	constructor(private http: HttpClient) { }

	getComboBoxData(query: string): Observable<any> {
		return this.http.get<Observable<any>>(
			this.baseUrl + 'Common/comboId?query=' + query
		);
	}

	getComboBoxstring(query: string): Observable<any> {
		return this.http.get<Observable<any>>(
			this.baseUrl + 'Common/comboIdstring?query=' + query
		);
	}

	getTableCommonData(table: string): Observable<DynamicTable[]> {
		return this.http.get<DynamicTable[]>(this.baseUrl + 'Common/' + table);
	}

}
