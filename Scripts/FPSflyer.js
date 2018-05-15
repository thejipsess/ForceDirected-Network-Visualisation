     
    var speed = 6.0;
    private var moveDirection = Vector3.zero;
     
     
    function FixedUpdate() {
     
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("UpDown"), Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
           
            var controller : CharacterController = GetComponent(CharacterController);
            var flags = controller.Move(moveDirection * Time.deltaTime);
                   
        }
     
    @script RequireComponent(CharacterController)
     
