import { Component, Inject } from '@angular/core';
import { MessageService } from './core/message-service';
import { DOCUMENT, Location} from '@angular/common';
import { AuthService } from '@auth0/auth0-angular';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})


export class AppComponent {

  constructor(private messages: MessageService, public location: Location, public auth: AuthService, @Inject(DOCUMENT) public document: Document) { }
  
  search(criteria: string){
      this.messages.sendSearchLists(criteria);
  }
}






