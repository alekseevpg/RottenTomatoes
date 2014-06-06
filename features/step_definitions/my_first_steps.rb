Given /^I am on the Welcome Screen$/ do

	wait_for(:timeout => 5) { element_exists("label text:'Top Box Office'") }
	touch("view marked: 'MovieCell-1-1'")

	wait_for(:timeout => 5) { element_exists("label text:'Movie info'") }
	# check_element_exists("view marked: 'Rotten'")
	check_element_does_not_exist("view marked: 'Rotten'")
	sleep(STEP_PAUSE)
end