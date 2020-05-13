import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS} from '@angular/common/http';
import { RouterModule} from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { FooterComponent } from './footer/footer.component';

import { UserListComponent } from './users/user-list/user-list.component';
import { UserDetailComponent } from './users/user-detail/user-detail.component';
import { TopicListComponent } from './topics/topic-list/topic-list.component';
import { TopicDetailComponent } from './topics/topic-detail/topic-detail.component';
import { PostListComponent } from './posts/post-list/post-list.component';
import { PostDetailComponent } from './posts/post-detail/post-detail.component';
import { PaginationComponent } from './pagination/pagination.component';

import { ReactiveFormsModule } from '@angular/forms';
import { LogInComponent } from './authorization/login/login.component'
import { JwtInterceptor } from './_helpers/jwt-interceptor';
import { ErrorInterceptor } from './_helpers/http-error-interceptor';
import { AuthGuard } from './_helpers/authGuard';
import { RegisterComponent } from './authorization/register/register.component';

import { PostNewComponent } from './posts/post-new/post-new.component';
import { ProfileComponent } from './profile/profile.component';
import { AdminGuard } from './_helpers/adminGuard';
import { AdminComponent } from './AdminPanel/admin.component';
import { AdminUsersComponent } from './AdminPanel/users/admin-users.component';

@NgModule({
    declarations: [
        AppComponent,
        PaginationComponent,
        NavMenuComponent,
        HomeComponent,
        FooterComponent,
        UserListComponent,
        TopicListComponent,
        PostListComponent,
        UserDetailComponent,
        TopicDetailComponent,
        PostDetailComponent,
        LogInComponent,
        RegisterComponent,
        PostNewComponent,
        ProfileComponent,
        AdminComponent,
        AdminUsersComponent
    ],
    imports: [
        BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
        HttpClientModule,
        ReactiveFormsModule,
        RouterModule.forRoot([
            { path: '', component: HomeComponent, pathMatch: 'full' },
            {
                path: 'users',
                component: UserListComponent,
                pathMatch: 'full',
            },

            {
                path: 'users/:id',
                component: UserDetailComponent,
                pathMatch: 'full',
                canActivate: [AuthGuard]
            },

            {
                path: 'topics',
                component: TopicListComponent,
                pathMatch: 'full'
            },

            {
                path: 'topics/:id',
                component: TopicDetailComponent,
                pathMatch: 'full'
            },

            {
                path: 'posts',
                component: PostListComponent,
                pathMatch: 'full'
            },

            {
                path: 'posts/:id',
                component: PostDetailComponent,
                pathMatch: 'full'
            },

            {
                path: 'newPost',
                component: PostNewComponent,
                pathMatch: 'full',
                canActivate: [AuthGuard]
            },

            {
                path: 'login',
                component: LogInComponent
            },

            {
                path: 'register',
                component: RegisterComponent
            },

            {
                path: 'profile',
                component: ProfileComponent,
                canActivate: [AuthGuard]
            },
            {
                path: 'admin',
                component: AdminComponent,
                canActivate: [AdminGuard],
                children: [
                    {
                        path: 'users',
                        component: AdminUsersComponent
                    }
                ]
            }
            //{ path: '**', redirectTo: '/' }
        ])
    ],
    providers: [
        { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
        { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
