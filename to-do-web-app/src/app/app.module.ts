import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { DashboardModule } from './dashboard/dashboard.module';
import { CoreModule } from './core/core.module';
import { CreateEditModule } from './create-edit/create-edit.module';
import { MatMenuModule } from '@angular/material/menu';
import { AuthModule, User } from '@auth0/auth0-angular';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthHttpInterceptor } from '@auth0/auth0-angular';
import { ToDoListShareModule } from './to-do-list-share/to-do-list-module';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    DashboardModule,
    CreateEditModule,
    ToDoListShareModule,
    CoreModule,
    MatMenuModule,
    AuthModule.forRoot({
      domain: 'dev-ly15t0g7.us.auth0.com',
      clientId: 'D5FMHLsEjFyLhzQniooP44hRWE2diEcv',
      audience: 'https://localhost:7117/api',
      redirectUri: 'https://to-do-web-app.azurewebsites.net/',
      scope: 'get:lists get:list search:lists add:list modify:list get:item add:item modify:item delete:list delete:item modify:list-position modify:item-position',
      // Specify configuration for the interceptor             
      httpInterceptor: {
        allowedList: [
          {
            // Match any request that starts 'https://YOUR_DOMAIN/api/v2/' (note the asterisk)
            uri: 'https://to-do-api.azurewebsites.net/api*',
            allowAnonymous: true,
            tokenOptions: {
              // The attached token should target this audience
              audience: 'https://localhost:7117/api',
              scope: 'get:lists get:list search:lists add:list modify:list get:item add:item modify:item delete:list delete:item modify:list-position modify:item-position',
            }
          }
        ]
      }
    }),
  ],
  providers: [{ provide: HTTP_INTERCEPTORS, useClass: AuthHttpInterceptor, multi: true }],
  bootstrap: [AppComponent]
})
export class AppModule { }
