import { Component, OnInit, ViewChild } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { QuillModules } from 'ngx-quill';
import { SendEmailRequest } from './models/email';
import { GetAuthDetails } from 'src/app/shared/models/methods';
import { errorNotification, successNotification } from 'src/app/pages/dashboards/shared/notifications/notification';
import { EmailHttpServiceService } from './service/email-http-service.service';

@Component({
  selector: 'app-compose-mail',
  templateUrl: './compose-mail.component.html',
  styleUrls: ['./compose-mail.component.scss']
})
export class ComposeMailComponent implements OnInit {
  mailTo: string = '';
  mailCC: string = '';
  mailBCC: string = '';
  mailSubject: string = '';
  mailBody: string = '';
  quillConfig: QuillModules = {};

  @ViewChild('content', { static: true }) content: any;

  constructor (public activeModal: NgbModal, private emailHttpService: EmailHttpServiceService) { }


  ngOnInit(): void {
    this.quillConfig = {
      toolbar: [
        [{ font: [] }, { size: [] }],
        ["bold", "italic", "underline"],
        [{ script: "super" }, { script: "sub" }],
        [{ list: "ordered" }, { list: "bullet" }]
      ]
    }

    this.mailBody = `
    <p>Enter your email by editing using the available fields above!</p>`
  }

  openModal(): void {
    this.activeModal.open(this.content, { centered: true });
  }


  sendEmail(): void {
    const data: SendEmailRequest = {
      authDto: GetAuthDetails(),
      toEmail: this.mailTo,
      subject: this.mailSubject,
      body: this.mailBody
    };

    if (this.mailTo == "" || this.mailSubject == "" || this.mailBody == "") {
      errorNotification("Please fill in all fields!");
      return;
    }

    this.emailHttpService.sendEmail(data).subscribe({
      next: (response) => {
        if (response.isSuccess == true) {
          successNotification(response.message);
          this.activeModal.dismissAll();
        } else {
          errorNotification("Email failed to send!");
          this.activeModal.dismissAll();
        }
      },
      error: (error) => {
        errorNotification("Email failed to send!");
        this.activeModal.dismissAll();
      }
    });
  }
}
