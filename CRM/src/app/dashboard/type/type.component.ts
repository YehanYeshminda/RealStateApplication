import { Component, Input, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-type',
  templateUrl: './type.component.html',
  styleUrls: ['./type.component.scss']
})
export class TypeComponent {
  isType: boolean = false;
  isSource: boolean = false;
  isLeadStatus: boolean = false;
  isPreffered: boolean = false;
  isDesignation: boolean = false;
  isIssuedTo: boolean = false;
  isSalesby: boolean = false;
  isMedia: boolean = false;
  isExpense: boolean = false;
  isMain: boolean = false;
  isSub: boolean = false;
  isAgree :  boolean = false;

  isProptype: boolean = false;
  isPropcat: boolean = false;
  isPropsubcat: boolean = false;

  constructor(public bsModalRef: BsModalRef) { }
}
