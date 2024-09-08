import { Component, Input, OnInit } from '@angular/core';

// types
import { Member } from './member.model';
import { EmployeePerformance } from 'src/app/pages/dashboards/employee-performance/models/performance';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-widget-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.scss']
})
export class MemberCardComponent implements OnInit {
  public baseUrl = environment.signalR + "upload/staff/"

  // https://localhost:7037/upload/staff/

  @Input() member?: EmployeePerformance;

  constructor () { }

  ngOnInit(): void {
    console.log(this.baseUrl)
  }

}
