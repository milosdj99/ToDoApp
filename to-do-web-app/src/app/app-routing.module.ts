import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateEditComponent } from './create-edit/create-edit.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AuthGuardService } from './route-guards/auth-guard-service';
import { ToDoListShareComponent } from './to-do-list-share/to-do-list-share.component';

const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: 'dashboard', component: DashboardComponent},
  { path: 'toDoLists/:id', component: CreateEditComponent, canActivate: [AuthGuardService]},
  { path: 'toDoLists', component: CreateEditComponent, canActivate: [AuthGuardService] },
  { path: 'share/:id', component: ToDoListShareComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
