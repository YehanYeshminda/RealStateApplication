import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-email-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.scss']
})
export class MessageComponent implements OnInit {

  @Output() closeMessage = new EventEmitter<void>();
  @Input() body: string = ''
  @Input() subject: string = ''
  @Input() fromEmail: string = 'yeshanyesh12@gmail.com'
  @Input() time: Date = new Date()

  constructor () { }

  ngOnInit(): void {
  }

  handleMessageClose(): void {
    this.closeMessage.emit();
  }

}
