import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { ToDoPreviewComponent } from "./to-do-preview/to-do-preview.component";
import { DashboardComponent } from "./dashboard.component";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { MatButtonModule } from "@angular/material/button";
import {MatToolbarModule} from '@angular/material/toolbar';
import {MatListModule} from '@angular/material/list'
import {MatCardModule} from '@angular/material/card';
import {MatCheckboxModule} from '@angular/material/checkbox';
import { DragDropModule } from "@angular/cdk/drag-drop";
import { MatInputModule } from "@angular/material/input";
import { MatDividerModule } from '@angular/material/divider';


@NgModule({
    declarations: [
        DashboardComponent,
        ToDoPreviewComponent
    ],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        MatButtonModule,
        MatToolbarModule,
        MatListModule,
        MatCardModule,
        MatCheckboxModule,
        DragDropModule,
        MatInputModule,
        MatDividerModule
    ],
    exports: [
        DashboardComponent
    ]
})
export class DashboardModule { }