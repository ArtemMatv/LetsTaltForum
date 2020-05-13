import { Component, OnInit } from '@angular/core';
import { PostService } from '../../_services/post-service';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { TopicService } from '../../_services/topic-service';
import { formatDate } from '@angular/common';
import { NavbarFooterService } from '../../_services/nav-service';

@Component({
    selector: 'app-post-new',
    templateUrl: './post-new.component.html',
    styleUrls: ['./post-new.component.css']
})
export class PostNewComponent implements OnInit {
    private topicNames: string[]

    constructor(private postService: PostService,
        private topicService: TopicService,
        private formBuilder: FormBuilder,
        private router: Router,
        private nav: NavbarFooterService
    ) {
        this.topicNames = topicService.getNames()
    }

    postForm: FormGroup;
    loading = false;
    submitted = false;
    error = '';

    ngOnInit() {
        this.postForm = this.formBuilder.group({
            topic: ['', Validators.required],
            title: ['', Validators.required],
            message: ['', Validators.required]
        });
        this.nav.hide()
    }

    // convenience getter for easy access to form fields
    get f() { return this.postForm.controls; }

    onSubmit() {
        this.submitted = true;

        // stop here if form is invalid
        if (this.postForm.invalid) {
            return;
        }

        this.loading = true;


        let date = formatDate(Date.now(), "MM.dd.yyyy HH:mm", 'en-US')

        this.postService.createPost(
            this.topicService.getIdByName(this.f.topic.value),
            this.f.title.value,
            this.f.message.value,
            JSON.parse(localStorage.getItem('currentUser')).id,
            date
        )
        
        this.router.navigate(['/profile'])
    }
}
