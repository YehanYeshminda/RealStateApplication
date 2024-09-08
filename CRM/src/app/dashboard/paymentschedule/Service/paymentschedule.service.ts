import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthDetails } from 'src/app/shared/models/authDetails';
import { Root } from 'src/app/shared/models/baseResponse';
import { environment } from 'src/environments/environment.development';
import { PaymentSchedule, PaymentScheduleList } from '../models/paymentschedule';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PaymentscheduleService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  addPaymentSchedule(data: PaymentSchedule): Observable<Root<PaymentSchedule>> {
    return this.http.post<Root<PaymentSchedule>>(this.baseUrl + "PaymentSchedule/AddPaymentSchedule", data)
  }

  editPaymentSchedule(data: PaymentSchedule): Observable<Root<PaymentSchedule>> {
    return this.http.put<Root<PaymentSchedule>>(this.baseUrl + "PaymentSchedule/EditPaymentSchedule", data)
  }

  getAllPaymentSchedule(auth: AuthDetails): Observable<Root<PaymentScheduleList[]>> {
    return this.http.post<Root<PaymentScheduleList[]>>(this.baseUrl + "PaymentSchedule/GetAllPaymentSchedule", auth)
  }

  getPaymentScheduleById(id: number, auth: AuthDetails): Observable<PaymentSchedule> {
    return this.http.post<PaymentSchedule>(
      this.baseUrl + 'PaymentSchedule/GetPaymentScheduleById/' + id,
      auth
    );
  }
}
