package id.ctrlsoft.catatuang.connection.model;

import java.io.Serializable;

import id.ctrlsoft.catatuang.repository.model.User;

public class Callbacks implements Serializable {
    public String JSONMessage = "";
    public long JSONRows = 0l;
    public boolean JSONResult = false;
    public Object JSONValue;
}

