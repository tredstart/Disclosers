import pyrebase as pyrebase
from django.http import JsonResponse, HttpResponse
from django.shortcuts import render
import ast

# Create your views here.
from dbase.models import BaseObject


config = {
    "apiKey": "AIzaSyBHJa2wPkBpklLoSnPM_l_hYgw7NcY5XA0",
    "authDomain": "bintween-e0b79.firebaseapp.com",
    "databaseURL": "https://bintween-e0b79.firebaseio.com",
    "projectId": "bintween-e0b79",
    "storageBucket": "bintween-e0b79.appspot.com",
    "messagingSenderId": "241454414956",
    "appId": "1:241454414956:web:2ad237ab9bb2b795576c49"
}

firebase = pyrebase.initialize_app(config)
db = firebase.database()


def getJSONdata(request, **kwargs):
    db_name = kwargs['dbname']
    cross_db = db.child(db_name).get()
    print(cross_db.val())
    if isinstance(cross_db.val(), dict):
        return JsonResponse(cross_db.val())
    else:
        return HttpResponse(cross_db.val())


def setJSONdata(request, **kwargs):
    db_name = kwargs['dbname']
    keys = kwargs['key'].split('-')
    new_keys = "/".join(keys[:-1])
    data = {keys[-1]: kwargs['value']}

    database_data = db_name + "/" + new_keys
    db.child(database_data).update(data)
    print(database_data, data, "<-- the data")
    return JsonResponse(db.child(db_name).get().val())


def getValueFromJSON(request, **kwargs):
    db_name = kwargs['dbname']
    keys = kwargs['key'].split('-')
    keys = "/".join(keys)

    database_data = db.child(db_name+"/"+keys).get().val()
    print(database_data)
    if isinstance(database_data, dict):
        return JsonResponse(database_data)
    else:
        return HttpResponse(database_data)
