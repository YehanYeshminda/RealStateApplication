import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Observable } from 'rxjs';
import { liveNotificationResponse } from 'src/app/core/top-nav/models/notificationData';
import { getAuthDetails } from 'src/app/shared/methods/method';
import { AuthDetails } from 'src/app/shared/models/authDetails';
import { Root } from 'src/app/shared/models/baseResponse';
import { environment } from 'src/environments/environment.development';


@Injectable({
  providedIn: 'root'
})
export class NotificationHttpService {
  baseUrl = environment.apiUrl;

  // hubConnection!: HubConnection;

  constructor(private http: HttpClient) {
    // this.hubConnection = new HubConnectionBuilder()
    //   .withUrl(environment.signalR + 'myhub')
    //   .build();

    // this.startConnection();
  }

  // private startConnection() {
  //   this.hubConnection
  //     .start()
  //     .then(() => {
  //       console.log('Connection started');
  //     })
  //     .catch((err) => {
  //       console.error('Error while starting SignalR connection: ' + err);
  //     });
  // }

  getAllNotificatiosForUser(): Observable<Root<liveNotificationResponse[]>> {
    const auth: AuthDetails = getAuthDetails();
    return this.http.post<Root<liveNotificationResponse[]>>(this.baseUrl + "Notification/GetNotificationForUser", auth);
  }

  getNotifications(): Observable<Root<number>> {
    const auth: AuthDetails = getAuthDetails();
    return this.http.post<Root<number>>(this.baseUrl + "Notification", auth)
  }

  getNoficationCountForUser() {
    const auth: AuthDetails = getAuthDetails();
    return this.http.post<Root<number>>(this.baseUrl + "Notification/UpdateNotificationCount", auth);
  }
}
